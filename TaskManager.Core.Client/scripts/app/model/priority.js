System.register([], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var Priority;
    return {
        setters: [],
        execute: function () {
            Priority = class Priority {
                constructor(id, name) {
                    this.id = id;
                    this.name = name;
                }
            };
            exports_1("Priority", Priority);
        }
    };
});
