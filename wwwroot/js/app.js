(function () {
    var urlShortnerApp = angular.module('urlShortnerApp', ['ngRoute']);

    urlShortnerApp.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/', {
                templateUrl: '../html/shortner.html'
            }).otherwise({ redirectTo: '/' });
    }]);
}());