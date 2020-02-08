Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("ws");
WebSocket = ws;
var sapphiredb_1 = require("sapphiredb");
var operators_1 = require("rxjs/operators");
var moment = require("moment");
var axios_1 = require("axios");
var uuid = require("uuid/v4");
var clientId = uuid();
var db = new sapphiredb_1.SapphireDb({
    serverBaseUrl: 'sapphire-perf-server.azurewebsites.net',
    useSsl: true
});
var collection = db.collection('entries');
var data = [];
collection.values().pipe(operators_1.filter(function (v) { return v.length === 1; }), operators_1.skip(1), operators_1.map(function (v) { return v[0]; })).subscribe(function (newValue) {
    var currentDate = moment();
    var createdDateServer = moment(newValue.createdOn);
    var diffFromServer = currentDate.diff(createdDateServer, 'milliseconds');
    var createdDateClient = moment(newValue.createdOnClient);
    var diffFromClient = currentDate.diff(createdDateClient, 'milliseconds');
    data.push({
        clientId: clientId,
        diffFromServer: diffFromServer,
        diffFromClient: diffFromClient,
        receivedOn: currentDate
    });
    if (data.length >= 100) {
        console.log('sending data to server');
        axios_1.default.post('https://sapphire-perf-data-server.azurewebsites.net/data/postData', data);
        data = [];
    }
    console.log('server diff', diffFromServer, 'client diff', diffFromClient);
});
