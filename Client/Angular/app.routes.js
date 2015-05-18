angular
	.module('app')
  .config(['$routeProvider', configureRoutes]);

function configureRoutes($routeProvider) {
	$routeProvider
		.when('/Posts', {
			templateUrl: '/Angular/Posts/Index.html',
			controller: 'PostsController',
      controllerAs: 'vm'
		})
		.when('/Account/Login', {
			templateUrl: '/Angular/Account/Login.html',
			controller: 'LoginController',
      controllerAs: 'vm'
		})
		.when('/Account/Register', {
			templateUrl: '/Angular/Account/Register.html',
			controller: 'RegisterController',
      controllerAs: 'vm'
		})
		.otherwise({
			redirectTo: '/Posts'
		});
}
