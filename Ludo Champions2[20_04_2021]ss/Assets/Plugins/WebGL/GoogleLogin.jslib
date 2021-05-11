var googleLogin = {
	$googleInstances: [],
    login: function(objectNamePtr, funcNameSuccessPtr) {
      // Because unity is currently bad at JavaScript we can't use standard
      // JavaScript idioms like closures so we have to use global variables :(
      window.becauseUnitysBadWithJavacript_getImageFromBrowser =
          window.becauseUnitysBadWithJavacript_getImageFromBrowser || {
         busy: false,
         initialized: false,
         rootDisplayStyle: null,  // style to make root element visible
         root_: null,             // root element of form
         ctx_: null,              // canvas for getting image data;
      };
      var g = window.becauseUnitysBadWithJavacript_getImageFromBrowser;
      if (g.busy) {
          // Don't let multiple requests come in
          return;
      }
      g.busy = true;

      var objectName = Pointer_stringify(objectNamePtr);
      var funcNameSuccess = Pointer_stringify(funcNameSuccessPtr);
     // var funcNameFailer = Pointer_stringify(funcNameFailerPtr);
	  
	  if (!g.initialized) {
		  
		//Load google login api for javascript
		loadScript("https://apis.google.com/js/platform.js",function(){
			  
			console.log("Google Login Plugin Loaded..........");
			
			//var auth;
			gapi.load('auth2', function() {
				gapi.auth2.init({
					client_id: "1052677144191-hfqc8dq06hlp3v0ao623d0iioh9u1vh1.apps.googleusercontent.com",
					scope: "profile email" // this isn't required
				}).then(function(auth2) {
			
					//gapi.auth2.getAuthInstance();
					//auth=auth2;
					console.log("gapi Load Complete..."+gapi);
					var ind=googleInstances.push(gapi);
					console.log("gapi Load Complete.."+ind);
					console.log( "signed in: " + auth2.isSignedIn.get() ); 
					if(auth2.isSignedIn){
						console.log("" + "isSignedIn.."+auth2.isSignedIn);
						onSignIn(auth2.currentUser.get());
					}				
					auth2.isSignedIn.listen(onSignInNew);
				});
			});
			
			function isRealValue(obj)
			{
				return obj && obj != 'null' && obj !== undefined;
			};
		
			function onSignIn(googleUser) {
		
				// Useful data for your client-side scripts:
				if(isRealValue(googleUser)){
					console.log("onSignIn isRealValue......");
					var profile = googleUser.getBasicProfile();
				
					if(isRealValue(profile)){
						console.log("" + profile.getId());
						sendSuccessResult("{\"id\":\""+profile.getId()+"\",\"status\":true,\"event_name\":\"g_signup\",\"email\":\""+profile.getEmail()+"\",\"displayName\":\""+profile.getName()+"\",\"givenName\":\""+profile.getGivenName()+"\",\"familyName\":\""+profile.getFamilyName()+"\",\"photoUrl\":\""+profile.getImageUrl()+"\",\"obfuscatedIdentifier\":\"\",\"expirationTime\":0,\"grantedScopes\":[\"https://www.googleapis.com/auth/plus.me\",\"https://www.googleapis.com/auth/userinfo.email\",\"https://www.googleapis.com/auth/userinfo.profile\",\"profile\",\"email\",\"openid\"]}");
						
						//console.log("User Login:---------- ");
						
					}else{
						console.log("" + "User Logouted...");
					}
				
				}else{
					console.log("User Logout already: ");
				}
			};
		
			function onSignInNew(googleUser) {
				console.log("onSignInNew......");
				var auth2=gapi.auth2.getAuthInstance();
				if(auth2.isSignedIn){
					console.log("" + "isSignedIn.."+auth2.isSignedIn);
					onSignIn(auth2.currentUser.get());
				}	
			};
		 });
		 
		 //Load Library end here
		  
		  //Add manual HTML for user click
          g.initialized = true;
          g.ctx = window.document.createElement("canvas").getContext("2d");

          // Append a form to the page (more self contained than editing the HTML?)
          g.root = window.document.createElement("div");
          g.root.innerHTML = [
            '<style>                                                    ',
            '.login {                                                ',
            '    position: absolute;                                    ',
            '    left: 0;                                               ',
            '    top: 0;                                                ',
            '    width: 100%;                                           ',
            '    height: 100%;                                          ',
            '    display: -webkit-flex;                                 ',
            '    display: flex;                                         ',
            '    -webkit-flex-flow: column;                             ',
            '    flex-flow: column;                                     ',
            '    -webkit-justify-content: center;                       ',
            '    -webkit-align-content: center;                         ',
            '    -webkit-align-items: center;                           ',
            '                                                           ',
            '    justify-content: center;                               ',
            '    align-content: center;                                 ',
            '    align-items: center;                                   ',
            '                                                           ',
            '    z-index: 2;                                            ',
            '    color: white;                                          ',
            '    background-color: rgba(200,0,0,0.8);                     ',
            '    font: sans-serif;                                      ',
            '    font-size: x-large;                                    ',
            '}                                                          ',
            '.login a,                                               ',
            '.login label {                                          ',
            '   font-size: x-large;                                     ',
            '   background-color: #666;                                 ',
            '   border-radius: 0.5em;                                   ',
            '   border: 1px solid black;                                ',
            '   padding: 0.5em;                                         ',
            '   margin: 0.25em;                                         ',
            '   outline: none;                                          ',
            '   display: inline-block;                                  ',
            '}                                                          ',
            '.login input {                                          ',
            '    display: none;                                         ',
            '}                                                          ',
            '</style>                                                   ',
            '<div class="login">                                     ',
            '    <div>                                                  ',
            '      <label for="Login">Google Login</label>  ',
            '     <br/>',
            '      <a>Cancel</a>                                        ',
            '    </div>                                                 ',
            '</div>                                                     ',
          ].join('\n');
          
		  
          // prevent clicking in input or label from canceling
          
          var label = g.root.querySelector("label");
          label.addEventListener('click', preventOtherClicks);

          // clicking cancel or outside cancels
          var cancel = g.root.querySelector("a");  // there's only one
          cancel.addEventListener('click', handleCancel);
          var loginCss = g.root.querySelector(".login");
          loginCss.addEventListener('click', handleCancel);

          // remember the original style
          g.rootDisplayStyle = g.root.style.display;

          window.document.body.appendChild(g.root);
		
		}
		console.log("Google User init..........");
		
		// make it visible
		g.root.style.display = g.rootDisplayStyle;

      function preventOtherClicks(evt) {
		  googleInstances[0].auth2.getAuthInstance().signIn();
          evt.stopPropagation();
      }

      function handleCancel(evt) {
          evt.stopPropagation();
          evt.preventDefault();
          sendFailer("{\"status\":false,\"event_name\":\"g_signup\"}");
      }

      function hide() {
          g.root.style.display = "none";
      }

      function sendSuccessResult(result) {
          hide();
          g.busy = false;
		  console.log("sendSuccessResult.........."+result);
          SendMessage(objectName, funcNameSuccess, result);
      }
	  
	  function sendFailer(result) {
          hide();
          g.busy = false;
		  console.log("sendFailer.........."+result);
          SendMessage(objectName, funcNameSuccess, result);
      }
		
	function loadScript(url, callback)
		{
			// Adding the script tag to the head as suggested before
			var head = document.head;
			var script = document.createElement('script');
			script.type = 'text/javascript';
			script.src = url;

				// Then bind the event to the callback function.
			// There are several events for cross browser compatibility.
			script.onreadystatechange = callback;
			script.onload = callback;

			// Fire the loading
			head.appendChild(script);
		}

	  
    },openUrl: function(url) {
		var urlString = Pointer_stringify(url);
		var OpenPopup = function() {
			window.open(urlString, null, 'width=500,height=500');
			document.getElementById('canvas').removeEventListener('click', OpenPopup);
		};
		document.getElementById('canvas').addEventListener('click', OpenPopup, false);
		
		console.log("login Start:**************** ");
	},logout: function(objectNamePtr, funcNameSuccessPtr) {
		
       
		var objectName = Pointer_stringify(objectNamePtr);
		var funcNameSuccess = Pointer_stringify(funcNameSuccessPtr);
		
		if(!isRealValue(googleInstances[0])){
				
				//Load google login api for javascript
		loadScript("https://apis.google.com/js/api.js",function(){
			  
			console.log("Google Init Plugin Loaded..........");
			
			//var auth;
			gapi.load('auth2', function() {
				gapi.auth2.init({
					client_id: "1052677144191-hfqc8dq06hlp3v0ao623d0iioh9u1vh1.apps.googleusercontent.com",
					scope: "profile email" // this isn't required
				}).then(function(auth2) {
						auth2.signOut();
						auth2.disconnect();	
						console.log("Google User Logout Called JS..........");
						SendMessage(objectName, funcNameSuccess, "{\"status\":true}");
				});
			});
			
		 });
			return;
		}else{
			 
			 var auth2= googleInstances[0].auth2.getAuthInstance();
		auth2.signOut();
		auth2.disconnect();	
		
		
		console.log("Google User Logout Called JS..........");
		SendMessage(objectName, funcNameSuccess, "{\"status\":true}");
		}
		
		
		
		
		function isRealValue(obj)
			{
				return obj && obj != 'null' && obj !== undefined;
			};
			
			
			function loadScript(url, callback)
		{
			// Adding the script tag to the head as suggested before
			var head = document.head;
			var script = document.createElement('script');
			script.type = 'text/javascript';
			script.src = url;

				// Then bind the event to the callback function.
			// There are several events for cross browser compatibility.
			script.onreadystatechange = callback;
			script.onload = callback;

			// Fire the loading
			head.appendChild(script);
		}
    }
};
autoAddDeps(googleLogin, '$googleInstances');
mergeInto(LibraryManager.library, googleLogin);