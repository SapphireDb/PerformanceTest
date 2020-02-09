import * as ws from 'ws';
import {DefaultCollection, SapphireDb} from 'sapphiredb';
import {switchMap, take} from 'rxjs/operators';
import {of} from 'rxjs';
import {entryCount, perfServerUrl, useSsl} from './consts';

WebSocket = ws;

const db = new SapphireDb({
    serverBaseUrl: perfServerUrl,
    useSsl: useSsl
});

const collection: DefaultCollection<any> = db.collection('entries');

const runFunction = () => {
    collection.values().pipe(
        take(1),
        switchMap(v => {
            if (v.length > 0) {
                console.log(`Removing ${v.length} entries`);
                return collection.remove(...v);
            }

            return of(null);
        })
    ).subscribe(() => {
        const newData = new Array(entryCount).fill(null).map(() => ({ createdOnClient: new Date() }));
        collection.add(...newData).subscribe(() => {
            console.log(`Added ${newData.length} entries`);
            setTimeout(() => {
                runFunction();
            }, 2000);
        });
    });
};

runFunction();
