import * as ws from 'ws';
import 'linq4js';
import {DefaultCollection, SapphireDb} from 'sapphiredb';
import * as uuid from 'uuid/v4';
import {entriesCount, perfServerUrl, useSsl} from './consts';
import {catchError, filter, skip, take} from 'rxjs/operators';
import {of} from 'rxjs';

WebSocket = ws;

const clientId = uuid();

const db = new SapphireDb({
    serverBaseUrl: perfServerUrl,
    useSsl: useSsl
});

let collection: DefaultCollection<any>;

let startTime: [ number, number];

let lastEntries: { time: Date, diff: number }[] = [];

const createEntry = () => {
    collection.values().pipe(filter(v => v.length > 0), take(1)).subscribe(v => {
        const [ endTimeS, endTimeNs ] = process.hrtime(startTime);
        const diff = (endTimeS * 1000000000 + endTimeNs) / 1000000;

        console.log(`Id: ${clientId}; Diff: ${diff} ms`);

        lastEntries.push({ time: new Date(), diff: diff });

        collection.remove(...v).pipe(catchError(() => of(null))).subscribe(() => {
            if (lastEntries.length >= entriesCount) {
                console.log(`Id: ${clientId}; Sending data to server`);
                db.execute('message.received', clientId, new Date(), lastEntries).subscribe(() => {
                    lastEntries = [];
                    setTimeout(() => {
                        createEntry();
                    }, 2000);
                });
            } else {
                setTimeout(() => {
                    createEntry();
                }, 2000);
            }
        });
    });

    startTime = process.hrtime();

    collection.add({
        clientId: clientId
    });
};

const init = async () => {
    collection = db.collection('entries').where(['clientId', '==', clientId]);

    createEntry();
    // db.messaging.messages().subscribe(message => {
    //     console.log(`Id: ${clientId}; Received message`);
    //     db.execute('message.received', message, clientId);
    // });
};

init();
