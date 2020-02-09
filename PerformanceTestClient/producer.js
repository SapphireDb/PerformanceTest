Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("ws");
var sapphiredb_1 = require("sapphiredb");
var operators_1 = require("rxjs/operators");
var rxjs_1 = require("rxjs");
var consts_1 = require("./consts");
WebSocket = ws;
var db = new sapphiredb_1.SapphireDb({
    serverBaseUrl: consts_1.perfServerUrl,
    useSsl: consts_1.useSsl
});
var collection = db.collection('entries');
var runFunction = function () {
    collection.values().pipe(operators_1.take(1), operators_1.switchMap(function (v) {
        if (v.length > 0) {
            console.log("Removing " + v.length + " entries");
            return collection.remove.apply(collection, v);
        }
        return rxjs_1.of(null);
    })).subscribe(function () {
        var newData = new Array(consts_1.entryCount).fill(null).map(function () { return ({ createdOnClient: new Date() }); });
        collection.add.apply(collection, newData).subscribe(function () {
            console.log("Added " + newData.length + " entries");
            setTimeout(function () {
                runFunction();
            }, 2000);
        });
    });
};
runFunction();
