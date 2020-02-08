import * as ws from 'ws';
WebSocket = ws;
import {DefaultCollection, SapphireDb} from 'sapphiredb';
import {take} from 'rxjs/operators';

const db = new SapphireDb({
    serverBaseUrl: 'localhost:5000',
    useSsl: false
});

const collection: DefaultCollection<any> = db.collection('entries');

collection.values().pipe(take(1)).subscribe(v => {
    if (v.length > 0) {
        collection.remove(...v);
    }

    collection.add({});
});
