# High Performance ASP.NET Core Workshop

This repository contains a sample Web API application used in the [High-Performance ASP.NET Core Workshop](https://github.com/ironcev/public-talks/tree/master/HighPerformanceAspDotNetCore).

The Web API represents a simplified implementation of a "Getting Things Done (GTD)" system. To learn more about GTD, read the Erlend Hamberg's excellent article [GTD in 15 minutes – A Pragmatic Guide to Getting Things Done](https://hamberg.no/gtd/).

The API supports the following:

- Creating, editing and deleting of Actions, Lists and Projects.
- Moving an Action to a List.
- Assigning an Action to a Project.

The application does not provide a UI. To smoke test the API you can use any REST client. The [smoke tests](tests/smoke) folder contains a set of basic smoke tests given in a [plain text format](tests/smoke/PlainText.txt), or as files that can be imported into [Insomnia](https://insomnia.rest/) or [Postman](https://www.getpostman.com/).

## License
[![CC0](http://mirrors.creativecommons.org/presskit/buttons/88x31/svg/cc-zero.svg)](http://creativecommons.org/publicdomain/zero/1.0)

To the extent possible under law, Dobriša Adamec and Igor Rončević have waived all copyright and related or neighboring rights to this work.

In case of (re)use, providing a link to this GitHub repository would be highly appreciated.