angular
  .module('app')
  .controller('PostsController', PostsController);

PostsController.$inject = ['$location', 'workbench', '$q'];

function PostsController($location, workbench, $q) {
  /* jshint validthis:true */
  var vm = this;
    
  vm.title   = 'Posts';

  workbench.getPosts().then(function(data) {
    vm.posts = data;
  });

  activate();

  function activate() {      

  }
}
