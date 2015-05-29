angular
  .module('app')
  .controller('PostController', PostController);

PostController.$inject = ['$location', '$route', 'workbench', '$q'];

function PostController($location, $route, workbench, $q) {
  /* jshint validthis:true */
  var vm = this;
    
  vm.title   = 'Post';

  workbench.getPost($route.current.params.post).then(function(data) {
    vm.post = data;
  });

  activate();

  function activate() {      

  }
}
