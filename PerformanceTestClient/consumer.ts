import * as ws from 'ws';
import 'linq4js';
import {DefaultCollection, SapphireDb} from 'sapphiredb';
import {filter, skip} from 'rxjs/operators';
import * as moment from 'moment';
import {default as axios} from 'axios';
import * as uuid from 'uuid/v4';
import {entryCount, perfDataServerUrl, perfServerUrl, useSsl} from './consts';

WebSocket = ws;

const clientId = uuid();

const db = new SapphireDb({
    serverBaseUrl: perfServerUrl,
    useSsl: useSsl
});

const collection: DefaultCollection<any> = db.collection('entries');

let data = [];

collection.values().pipe(
    filter(v => v.length === entryCount),
    skip(1)
).subscribe((newValues) => {
    const currentDate = moment();
    newValues = newValues.map(v => {
       v['createdOn'] = moment(v.createdOn);
       v['createdOnClient'] = moment(v.createdOnClient);
       v['diffFromServer'] = currentDate.diff(v.createdOn, 'milliseconds');
       v['diffFromClient'] = currentDate.diff(v.createdOnClient, 'milliseconds');
       return v;
    });

    // const averageServerDiff = newValues.Average(v => v.diffFromServer);
    const averageClientDiff = newValues.Average(v => v.diffFromClient);

    // console.log(`Id: ${clientId}; Average server diff: ${averageServerDiff}; Average client diff: ${averageClientDiff}`);

    db.execute<number>('date', 'GetServerDiff', newValues[0].createdOn).subscribe((diffResult) => {
        const serverDiff = diffResult.result;

        console.log(`Id: ${clientId}; Average server diff: ${serverDiff}; Average client diff: ${averageClientDiff}`);

        data.push({
            clientId: clientId,
            averageServerDiff: serverDiff,
            averageClientDiff: averageClientDiff,
            receivedOn: currentDate
        });

        if (data.length >= 100) {
            const dataToSend = data.slice(0);

            const sendData = () => {
                axios.post(`${perfDataServerUrl}/data/postData`, dataToSend).catch(() => {
                    sendData();
                }).then(() => {
                    console.log(`Id: ${clientId}; Sent data to server`);
                });
            };

            sendData();

            data = [];
        }
    });
});
