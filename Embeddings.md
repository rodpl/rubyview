You can use the same embeddings as in RoR, such as `<% %>`, `<% -%>` and `<%= %>`.

`<% %>` is for code block.
`<%= %>` renders string to the page

```
  <b>Names of all the people</b>
  <% for person in @people %>
    Name: <%= person.name %><br/>
  <% end %>
```

Using <% %> will generate us empty line in Html code. The same like in ASP.NET
```
Header
<%
  my_variable = 0
%>
Footer
```
... will generate such text
```
Header

Footer
```
To avoid such effect just use `<% -%>`
