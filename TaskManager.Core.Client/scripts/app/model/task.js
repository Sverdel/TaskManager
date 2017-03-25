System.register([], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var Task;
    return {
        setters: [],
        execute: function () {
            Task = class Task {
                constructor(id, name) {
                    this.id = id;
                    this.name = name;
                    this.stateId = 1;
                    this.priorityId = 1;
                }
            };
            exports_1("Task", Task);
        }
    };
});
