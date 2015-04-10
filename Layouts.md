Layouts are like master page in ASP.NET but with more flexibility. Any view can be embedded into the layout. Layouts are defined in Controller by LayoutAttribute or by setting LayoutNames property. You can use multiply layouts:

... sample controller
```
[Layout("First", "Second")]
public class MyController :Controller
{
    public void Index()
    {
...
```


... Views/MyController/index.erb
```
This is view
```


... Views/layouts/first.erb
```
First layout header
<%= yield %>
First layout footer
```


... Views/layouts/second.erb
```
Second layout header
<%= yield %>
Second layout footer
```


... will give us such effect:
```
First layout header
Second layout header
This is view
Second layout footer
First layout footer
```

The same layouts can be used many times in one page
```
[Layout("First", "First", "Second", "Second")]
public class MyController :Controller
{
    public void Index()
    {
...
```