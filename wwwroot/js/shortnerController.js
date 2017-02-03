(function () {
    'use strict';

    angular
        .module('urlShortnerApp')
        .controller('shortnerController', shortnerController);

    shortnerController.$inject = ['$scope','dataService']; 

    function shortnerController($scope, dataService) {
        
        $scope.shorten = function (url) {
            dataService.shortenUrl(url)
                .then(function (data) {
                    console.log(data);
                    $scope.shortUrl = data;
                }, function (error) {
                    alert(error);
                });
        };
    }
})();
