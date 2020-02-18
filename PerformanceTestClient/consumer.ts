import * as ws from 'ws';
import 'linq4js';
import {SapphireDb} from 'sapphiredb';
import * as uuid from 'uuid/v4';
import {perfServerUrl, useSsl} from './consts';

WebSocket = ws;

const clientId = uuid();

const db = new SapphireDb({
    serverBaseUrl: perfServerUrl,
    useSsl: useSsl
});

const init = async () => {
    db.messaging.messages().subscribe(message => {
        console.log(`Id: ${clientId}; Received message`);
        db.execute('message.received', message, clientId);
    });
};

init();
