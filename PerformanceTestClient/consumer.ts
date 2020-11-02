import * as ws from 'ws';
import 'linq4js';
import {DefaultCollection, SapphireDb} from 'sapphiredb';
import * as uuid from 'uuid/v4';
import {entriesCount, perfServerUrl, useSsl} from './consts';
import {filter, map, skip, takeUntil} from 'rxjs/operators';

WebSocket = ws;

const clientId = uuid();

const db = new SapphireDb({
    serverBaseUrl: perfServerUrl,
    useSsl: useSsl
});

let measurementEntries: { time: Date, received: Date }[] = [];

const collection: DefaultCollection<any> = db.collection('entries');
const values$ = collection.values();

const startListening = () => {
    values$.pipe(
        takeUntil(db.online().pipe(skip(1), filter(v => !v))),
        filter(v => v.length === 1),
        skip(2),
        map(v => v[0])
    ).subscribe({
        next: value => {
            measurementEntries.push({time: value.time, received: new Date()});
            console.log(`Id: ${clientId}; Received entry from ${value.time}`);

            if (measurementEntries.length >= entriesCount) {
                console.log(`Id: ${clientId}; Sending measurements to server`);
                db.execute('store.measurements', clientId, new Date(), measurementEntries).subscribe(() => {
                    console.log(`Id: ${clientId}; Measurements stored`);
                    measurementEntries = [];
                });
            }
        },
        complete: () => startListening()
    });

    console.log(`Id: ${clientId}; Started listening`);
};

startListening();
