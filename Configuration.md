### Installing ###
To use RubyViewEngine you don't need to have Ruby installed. Copy these libraries into your bin folder in MonoRail web site:
  * Castle.MonoRail.Views.RubyView.dll
  * IronRuby.dll
  * IronRuby.Libraries.dll
  * Microsoft.Scripting.dll
  * Microsoft.Scripting.Core.dll

### Configuring in web.config ###
Add engine definition in web.config in monorail section
```
<viewEngines>
	<add type="Castle.MonoRail.Views.RubyView.RubyViewEngine, Castle.MonoRail.Views.RubyView"/>
</viewEngines>
```

File extension for views is .erb. Its a good idea to add such handler in config.
```
<add verb="*" path="*.erb" type="System.Web.HttpForbiddenHandler"/>
```