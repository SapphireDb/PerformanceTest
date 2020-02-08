import * as ws from 'ws';
WebSocket = ws;
import {DefaultCollection, SapphireDb} from 'sapphiredb';
import {filter, map} from 'rxjs/operators';
import * as moment from 'moment';

const db = new SapphireDb({
    serverBaseUrl: 'localhost:5000',
    useSsl: false
});

const collection: DefaultCollection<any> = db.collection('entries');

collection.values().pipe(
    filter(v => v.length === 1),
    map(v => v[0])
).subscribe((newValue) => {
    const currentDate = moment();
    const createdDate = moment(newValue.createdOn);

    console.log(newValue);
    console.log(currentDate.diff(createdDate, 'milliseconds'));
});
