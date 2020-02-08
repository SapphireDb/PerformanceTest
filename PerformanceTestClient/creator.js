Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("ws");
WebSocket = ws;
var sapphiredb_1 = require("sapphiredb");
var operators_1 = require("rxjs/operators");
var db = new sapphiredb_1.SapphireDb({
    serverBaseUrl: 'localhost:5000',
    useSsl: false
});
var collection = db.collection('entries');
collection.values().pipe(operators_1.take(1)).subscribe(function (v) {
    console.log(v);
    if (v.length > 0) {
        collection.remove.apply(collection, v);
    }
    collection.add({});
});
