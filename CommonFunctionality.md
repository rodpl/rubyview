### Variables ###
You can use local variables in common way.
```
<% my_variable = 0 -%>
Variable should be 0 <%= my_variable %> 
<% my_variable = 5 -%>
Variable should be 5 <%= my_variable %> 
```

### Methods ###
Methods can be also used
```
<%
  def small_calculator(a,b)
    a + b
  end
%>
When 2 adds to 3 it will be <%= small_calculator(2,3) %>
```