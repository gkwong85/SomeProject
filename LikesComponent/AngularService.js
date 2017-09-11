(function () {
    "use strict";
    var app = angular.module('app');

    app.factory('likesService', likesService);
    likesService.$inject = ['$http', '$q', '$timeout'];

    function likesService($http, $q, $timeout) {
        var pendingIds = [];
        var deferred = null;

        function _getAllLikes(id) {
            pendingIds.push(id);
            if (!deferred) {
                deferred = $q.defer();
                $timeout(function () {
                    deferred.resolve(
                        $http({
                            url: 'apiUrl',
                            data: { contentIds: pendingIds },
                            method: 'POST'
                        })
                    );
                    deferred = null;
                    pendingIds = [];
                });
            }
            return deferred.promise.then(_getAllLikesSuccess);

            function _getAllLikesSuccess(response) {
                return response.data.item[id];
            }
        };

        function _toggleLike(contentId) {
            var settings = {
                url: '/apiUrl',
                data: contentId,
                method: 'POST'
            }
            return $http(settings);
        }

        return {
            getAllLikes: _getAllLikes,
            toggleLike: _toggleLike,
        }
    }
})();