# Original-Coder-Libraries
Libraries for .NET that provide a wide selection of features and capabilities.

Please check out my profressional blog at https://www.originalcoder.dev

I've been building reusable libraries and freameworks since the 1990s.  Since starting work in C# about 14 years ago I've built up a rather massive collection of libraries that I've started the process of converting into open source.  This project will be where the open source versions of those libraries reside.

Now now this should be considered an Alpha release.  I'm mostly uploading at this point to get the repository estabulished and to start figuring out the best way to manage this and have binaries posted on NuGet.

Much of the code I'll be including in the iniital alpha release is based on very clean and well documented library code I've been using for years.  But I've done a bit of refactoring and cleaning up in addition to modernizing the code.  A lot of the original code was written some years ago and didn't take advantage of newer C# features.  I have not yet transferred or written unit tests so I'm not certain everything works at the moment.  But it is good, high quality code so you're welcome to give it a try and if you find any bugs either let me know of submit a fix.

------------------------------------------
2019-08-05 Alpha-2

For an example of how the Layers architecture will be configured and implemented please see the LayerApiMockup project which is included in this solution.  Also see the "! READ ME.txt" file that is in the root directory of that project (also visiable via Visual Studio Solution Explorer).

Check-in with more code cenerated around the OriginalCoder.Layers library that implements high level support for advanced layering architectures.  I've implemented a few different architectures loosely based around the same principals over the years with good success.  Basically most software services implemented by businesses have a great deal in common.  My goal has been to create architectures that significantly improves consistency, reduce the amount of code (espicially boilerplate) that application develpoers need to write while providing hooks for advanced features, flexibility for unusual cases and not impacting performance negatively.  The code in this archive is probably something like my 3rd generation of such architecture and I think the best one to date.

This code is very much alpha though.  It comples, there aren't any compiler warnings and virtually no ReSharper warnings either.  So the code is clean and well documented, but as of this moment it is also UNTESTED CODE.  I'm checking this in to have a baseline and in case anyone is curious and wants to have a look.  But it needs a bit more work and needs to be used in a project or two and tested.

Note that it is best to think of this sort of architecture as something on the scale of Entity Framework or ASP.NET MVC.  The first time developers are exposed to those they don't "get it" yet because there is a learning curve.  This layered architecture is the much same way.  Based on past experience some developers won't get it or understand what is being done and why in this architecture until they've at least seen it in use or had hands-on experience with it.  But also based on past experience once reaching that point most developers will find this architecture to be very helpful and time saving.  Just have to get past the learning curve.  Eventually there will be some documentation and digrams to help with that, but as mentioned this is an alpha initial check-in.

One addition thing to note, compared to Entity Framework or ASP.NET MVC this architecture is designed to be more fleixible and configurable.  The best way to use this architecture is to have an architect or lead developer decide how they want to use the architecture/library in their project and then create some applicaiton specifric base classes (which inherit from the library classes) to fill in some of the implementation details and rule out some of the configuration options.  By doing this it makes implementation and use by application developers much easier and straight forward since they don't need to be aware of the options and flexibility built into the architecture which won't be used in the application.