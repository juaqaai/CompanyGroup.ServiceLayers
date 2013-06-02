(function () {
    var root = this;

    define3rdPartyModules();
    boot();

    //console.log('main.newsletter');

    function define3rdPartyModules() {
                define('jquery', [], function () { return root.jQuery; });
                define('amplify', [], function () { return root.amplify; });
                define('sammy', [], function () { return root.Sammy; });
    }

    function boot() {
        //require(['app.newsletter'], function (app) { companyGroup.newsletter.run('#/'); });
        companyGroup.newsletter.run('#/');
    }
})();
