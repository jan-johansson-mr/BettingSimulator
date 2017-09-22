# BettingSimulator

Welcome to my sample solution, a betting simulator. It is a .NET Core (v2.0) solution, with three projects.

The project **EventConsole** is an interactive application, where you can spawn wagers and runners (up to 1000),
set the database connection string and initiate races.

The project **RaceControl** is a non-interactive application, receiving racing data from the EventConsole, executing
the races and produces an output manifest (a digital signed XML document) with winners, payout and odds.

There is lot more information around each race than is displayed by the console applications, and can be fun to look
at if you want.

The final project **MessageContracts** defines the (XML) contracts used by the solution.

Have a look in the ![Betting Simulator Document](./DemoPoolBetting.md) to read more about the solution design.

Have fun!
