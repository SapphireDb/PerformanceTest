Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("ws");
require("linq4js");
var sapphiredb_1 = require("sapphiredb");
var operators_1 = require("rxjs/operators");
var moment = require("moment");
var axios_1 = require("axios");
var uuid = require("uuid/v4");
var consts_1 = require("./consts");
WebSocket = ws;
var clientId = uuid();
var db = new sapphiredb_1.SapphireDb({
    serverBaseUrl: consts_1.perfServerUrl,
    useSsl: consts_1.useSsl
});
var collection = db.collection('entries');
var data = [];
collection.values().pipe(operators_1.filter(function (v) { return v.length === consts_1.entryCount; }), operators_1.skip(1)).subscribe(function (newValues) {
    var currentDate = moment();
    newValues = newValues.map(function (v) {
        v['createdOn'] = moment(v.createdOn);
        v['createdOnClient'] = moment(v.createdOnClient);
        v['diffFromServer'] = currentDate.diff(v.createdOn, 'milliseconds');
        v['diffFromClient'] = currentDate.diff(v.createdOnClient, 'milliseconds');
        return v;
    });
    // const averageServerDiff = newValues.Average(v => v.diffFromServer);
    var averageClientDiff = newValues.Average(function (v) { return v.diffFromClient; });
    // console.log(`Id: ${clientId}; Average server diff: ${averageServerDiff}; Average client diff: ${averageClientDiff}`);
    db.execute('date', 'GetServerDiff', newValues[0].createdOn).subscribe(function (diffResult) {
        var serverDiff = diffResult.result;
        console.log("Id: " + clientId + "; Average server diff: " + serverDiff + "; Average client diff: " + averageClientDiff);
        data.push({
            clientId: clientId,
            averageServerDiff: serverDiff,
            averageClientDiff: averageClientDiff,
            receivedOn: currentDate
        });
        if (data.length >= 100) {
            var dataToSend_1 = data.slice(0);
            var sendData_1 = function () {
                axios_1.default.post(consts_1.perfDataServerUrl + "/data/postData", dataToSend_1).catch(function () {
                    sendData_1();
                }).then(function () {
                    console.log("Id: " + clientId + "; Sent data to server");
                });
            };
            sendData_1();
            data = [];
        }
    });
});
