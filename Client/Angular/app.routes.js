angular
	.module('app')
  .config(['$routeProvider', configureRoutes]);

function configureRoutes($routeProvider) {
	$routeProvider
		.when('/Posts', {
			templateUrl: '/Angular/Views/Posts/Index.html',
			controller: 'PostsController',
      controllerAs: 'vm'
		})
		.when('/Account/Login', {
			templateUrl: '/Angular/Views/Account/Login.html',
			controller: 'LoginController',
      controllerAs: 'vm'
		})
		.when('/Account/Register', {
			templateUrl: '/Angular/Views/Account/Register.html',
			controller: 'RegisterController',
      controllerAs: 'vm'
		})
		.otherwise({
			redirectTo: '/Posts'
		});
}
