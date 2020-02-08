Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("ws");
WebSocket = ws;
var sapphiredb_1 = require("sapphiredb");
var operators_1 = require("rxjs/operators");
var moment = require("moment");
var db = new sapphiredb_1.SapphireDb({
    serverBaseUrl: 'localhost:5000',
    useSsl: false
});
var collection = db.collection('entries');
collection.values().pipe(operators_1.filter(function (v) { return v.length === 1; }), operators_1.map(function (v) { return v[0]; })).subscribe(function (newValue) {
    var currentDate = moment();
    var createdDate = moment(newValue.createdOn);
    console.log(newValue);
    console.log(currentDate.diff(createdDate, 'milliseconds'));
});
