(function () {
    'use strict';

    angular.module('taskApp')
        .filter('fromDictionary', function () {
            return function (input, dic) {
                if (input == null) { return; }
                var filtered = dic.filter(function (item) {
                    return (item.ID !== undefined && item.ID === input) || (item.Id !== undefined && item.Id === input) || (item.id !== undefined && item.id === input);
                });

                return filtered && filtered[0] ? ((filtered[0].Name || filtered[0].name)) : null;
            };
        });

})();