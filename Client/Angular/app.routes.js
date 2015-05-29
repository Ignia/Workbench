angular
	.module('app')
  .config(['$routeProvider', configureRoutes]);

function configureRoutes($routeProvider) {
	$routeProvider
		.when('/Posts', {
			templateUrl: '/Angular/Posts/Posts.html',
			controller: 'PostsController',
      controllerAs: 'vm'
		})
		.when('/Posts/:post', {
		  templateUrl: '/Angular/Posts/Post.html',
		  controller: 'PostController',
		  controllerAs: 'vm'
		})
		.when('/Account/:action', {
		  templateUrl: '/Angular/Account/Account.html',
		  controller: 'AccountController',
      controllerAs: 'vm'
		})
		.otherwise({
			redirectTo: '/Posts'
		});
}
