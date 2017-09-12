(function () {
    "use strict";
    var app = angular.module('app');

    app.factory('likesService', likesService);
    likesService.$inject = ['$http', '$q', '$timeout'];

    function likesService($http, $q, $timeout) {
        var pendingIds = [];
        var deferred = null;

        // using multiplexing to queue up all ids before creating request to retrieve data from database
        function _getAllLikes(contentId) {
            pendingIds.push(contentId);
            if (!deferred) {
                // creating a defer promise to defer (or delay) a result until ready to send as payload to endpoint
                deferred = $q.defer();
                // creating anonymous function to run once all ids being called by controller is complete, using resolve()
                $timeout(function () {
                    deferred.resolve(
                        $http({
                            url: 'apiUrl',
                            data: { contentIds: pendingIds },
                            method: 'POST'
                        })
                    );
                    // reset values
                    deferred = null;
                    pendingIds = [];
                });
            }
            // returning the deferred.promise, deferred.promise() allows asynchronous functions to prevent other code from 
            // interfering with the progress or status of its internal request
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