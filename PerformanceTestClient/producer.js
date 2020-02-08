Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("ws");
WebSocket = ws;
var sapphiredb_1 = require("sapphiredb");
var operators_1 = require("rxjs/operators");
var rxjs_1 = require("rxjs");
var db = new sapphiredb_1.SapphireDb({
    serverBaseUrl: 'sapphire-perf-server.azurewebsites.net',
    useSsl: true
});
var collection = db.collection('entries');
rxjs_1.timer(0, 200).pipe(operators_1.switchMap(function () { return collection.values().pipe(operators_1.take(1)); }), operators_1.switchMap(function (v) {
    if (v.length > 0) {
        console.log("Removing " + v.length + " entries");
        return collection.remove.apply(collection, v);
    }
    return rxjs_1.of(null);
})).subscribe(function () {
    console.log('Added entry');
    collection.add({ createdOnClient: new Date() });
});
