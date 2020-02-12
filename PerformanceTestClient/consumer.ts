import * as ws from 'ws';
import 'linq4js';
import {ActionResult, DefaultCollection, SapphireDb} from 'sapphiredb';
import {filter, skip} from 'rxjs/operators';
import * as moment from 'moment';
import * as uuid from 'uuid/v4';
import {perfServerUrl, useSsl} from './consts';

WebSocket = ws;

const clientId = uuid();

const db = new SapphireDb({
    serverBaseUrl: perfServerUrl,
    useSsl: useSsl
});

const init = async () => {
    const collection: DefaultCollection<any> = db.collection('entries');

    let data = [];

    collection.values().pipe(
        filter(v => v.length === 1),
        skip(1)
    ).subscribe((newValues) => {
        const currentTime = moment().utc(false);
        const newValueCreatedOn = moment.utc(newValues[0].createdOn);

        data.push({
            createdOn: newValueCreatedOn,
            receivedOn: currentTime
        });

        console.log(`Id: ${clientId}; Received data with diff of ${currentTime.diff(newValueCreatedOn, 'milliseconds')} ms`);

        if (data.length >= 10) {
            const dataToSend = data.slice(0);
            data = [];

            db.execute('data', 'send', dataToSend, clientId);
        }
    });
};

init();
