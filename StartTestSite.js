String.prototype.format = function()
{
	var pattern = /\{\d+\}/g;
	var args = arguments;
	return this.replace(pattern, function(capture){ return args[capture.match(/\d+/)]; });
}

fso = new ActiveXObject("Scripting.FileSystemObject");
shell = new ActiveXObject("WScript.Shell");

webServerPath = "C:\\Program Files\\Common Files\\Microsoft Shared\\DevServer\\9.0\\webdev.webserver.exe";
ARGV = WScript.Arguments;
command = '"{0}" /port:{1} /path:{2} /vpath:/'.format(webServerPath, ARGV.Item(0), fso.GetAbsolutePathName(ARGV.Item(1)));
WScript.Echo(command);
shell.Run(command);
