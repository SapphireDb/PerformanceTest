import * as ws from 'ws';
import 'linq4js';
import {ActionResult, DefaultCollection, SapphireDb} from 'sapphiredb';
import {filter, skip, take} from 'rxjs/operators';
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
    const collection: DefaultCollection<any> = db.collection('entries')
        .where(['clientId', '==', clientId]);

    let data = [];

    collection.values().pipe(
        filter(v => v.length === 1),
        skip(1)
    ).subscribe((newValues) => {
        const receivedOn = moment();
        const createdOn = moment(newValues[0].createdOn);

        data.push({
            createdOn: createdOn,
            receivedOn: receivedOn
        });

        console.log(`Id: ${clientId}; Diff: ${receivedOn.diff(createdOn, 'milliseconds')} ms`);

        if (data.length >= 100) {
            const dataToSend = data.slice(0);
            data = [];

            db.execute('data', 'send', dataToSend, clientId, moment());
            console.log(`Id: ${clientId}; Storing data in db`);
        }
    });

    const createData = () => {
        collection.values().pipe(take(1)).subscribe((values) => {
            collection.remove(...values).subscribe(() => {
                collection.add({
                    clientId: clientId,
                    createdOn: moment()
                }).subscribe(() => {
                    console.log(`Id: ${clientId}; Created entry`);
                    setTimeout(() => {
                        createData();
                    }, 2000);
                });
            });
        });
    };

    createData();
};

init();
