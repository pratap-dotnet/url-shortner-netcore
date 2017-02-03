(function () {
    'use strict';

    angular
        .module('urlShortnerApp')
        .factory('dataService', dataService);

    dataService.$inject = ['$http'];

    function dataService($http) {
        function shortenUrl(url) {
            return $http.post('/api/shorten', {
                url: url
            }).then(function (response){
                return response.data;
            }, function (response) {
                return response;
            });
        };

        var service = {
            shortenUrl: shortenUrl
        };

        return service;
    }
})();