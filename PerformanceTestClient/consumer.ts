import * as ws from 'ws';
WebSocket = ws;
import {DefaultCollection, SapphireDb} from 'sapphiredb';
import {filter, map, skip} from 'rxjs/operators';
import * as moment from 'moment';
import { default as axios } from 'axios';
import * as uuid from 'uuid/v4';

const clientId = uuid();

const db = new SapphireDb({
    serverBaseUrl: 'sapphire-perf-server.azurewebsites.net',
    useSsl: true
});

const collection: DefaultCollection<any> = db.collection('entries');

let data = [];

collection.values().pipe(
    filter(v => v.length === 1),
    skip(1),
    map(v => v[0]),
).subscribe((newValue) => {
    const currentDate = moment();

    const createdDateServer = moment(newValue.createdOn);
    const diffFromServer = currentDate.diff(createdDateServer, 'milliseconds');

    const createdDateClient = moment(newValue.createdOnClient);
    const diffFromClient = currentDate.diff(createdDateClient, 'milliseconds');

    data.push({
        clientId: clientId,
        diffFromServer: diffFromServer,
        diffFromClient: diffFromClient,
        receivedOn: currentDate
    });

    if (data.length >= 100) {
        console.log('sending data to server');
        axios.post('https://sapphire-perf-data-server.azurewebsites.net/data/postData', data);
        data = [];
    }

    console.log('server diff', diffFromServer, 'client diff', diffFromClient);
});
