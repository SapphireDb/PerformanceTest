Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("ws");
require("linq4js");
var sapphiredb_1 = require("sapphiredb");
var uuid = require("uuid/v4");
var consts_1 = require("./consts");
var operators_1 = require("rxjs/operators");
WebSocket = ws;
var clientId = uuid();
var db = new sapphiredb_1.SapphireDb({
    serverBaseUrl: consts_1.perfServerUrl,
    useSsl: consts_1.useSsl
});
var measurementEntries = [];
var collection = db.collection('entries');
var values$ = collection.values();
var startListening = function () {
    values$.pipe(operators_1.takeUntil(db.online().pipe(operators_1.skip(1), operators_1.filter(function (v) { return !v; }))), operators_1.filter(function (v) { return v.length === 1; }), operators_1.skip(2), operators_1.map(function (v) { return v[0]; })).subscribe({
        next: function (value) {
            measurementEntries.push({ time: value.time, received: new Date() });
            console.log("Id: " + clientId + "; Received entry from " + value.time);
            if (measurementEntries.length >= consts_1.entriesCount) {
                console.log("Id: " + clientId + "; Sending measurements to server");
                db.execute('store.measurements', clientId, new Date(), measurementEntries).subscribe(function () {
                    console.log("Id: " + clientId + "; Measurements stored");
                    measurementEntries = [];
                });
            }
        },
        complete: function () { return startListening(); }
    });
    console.log("Id: " + clientId + "; Started listening");
};
startListening();
