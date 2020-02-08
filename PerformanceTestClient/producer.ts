import * as ws from 'ws';
WebSocket = ws;
import {DefaultCollection, SapphireDb} from 'sapphiredb';
import {switchMap, take} from 'rxjs/operators';
import {of, timer} from 'rxjs';

const db = new SapphireDb({
    serverBaseUrl: 'sapphire-perf-server.azurewebsites.net',
    useSsl: true
});

const collection: DefaultCollection<any> = db.collection('entries');

timer(0, 200).pipe(
    switchMap(() => collection.values().pipe(take(1))),
    switchMap(v => {
        if (v.length > 0) {
            console.log(`Removing ${v.length} entries`);
            return collection.remove(...v);
        }

        return of(null);
    })
).subscribe(() => {
    console.log('Added entry');
    collection.add({ createdOnClient: new Date() });
});
