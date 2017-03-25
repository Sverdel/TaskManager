System.register([], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var State;
    return {
        setters: [],
        execute: function () {
            State = class State {
                constructor(id, name) {
                    this.id = id;
                    this.name = name;
                }
            };
            exports_1("State", State);
        }
    };
});
