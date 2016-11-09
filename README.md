# clParse
Provides a framework for parsing command line arguments in C# console applications. Allows you to 
encapsulate your program code into classes rather than respond to command line arguments with massive 
switch statements in the Main function.  Program code can now reside where it belongs, in separate class
files.  This also allows for better testability.

The assumed command line argument structure looks for a single CommandArgument that tells the application
what it will be doing, and its behavior can be modified by SwitchArguments.  CommandArgument can
define its own response loop, and will have a Status property of Successful once it is done, allowing 
you to loop until this happens (or until the Status is not equal to Executing).

## Usage
Lorem ipsum blah blah.


## Examples

#### Success Expected

##### Call The New Item Command with Arguments
```
.\program.exe newitem /name:"New Item" /estimate:5 /complete
```
Execute the NewitemCommand CommandArgument with a NamedArgument called NameArgument and a Value of "New Item". 
Another NamedArgument called EstimateArgument gives the resulting new item an estimate of 5, and a 
SwitchArgument called CompleteArgument modifies the behavior to give the new item a status of Complete.
In this case, CommandArgument has no need for a loop, so it sets the status to Successful and the
console application terminates normally.

##### Call the Start Timer Command Loop
```
.\program.exe start /id:2 
```
Execute the StartCommand that sends a dummy message to start a timer, waits for a key, and when it is pressed, 
sends a stop command and exits successfully.  The IdArgument NamedArgument tells it to send the message with
an ID of the Value property.  StartCommand should have a list of required arguments that contains IdArgument.

##### Show Help
```
.\program.exe help
```
Calls the HelpCommand object which displays some helpful information to the user and exits.

### Failure Expected

##### No CommandArgument
```
.\program.exe
```
In this case, this is an error for our demo console app, so we will get the amount of CommandArgument objects
in the argument list, and if 0, show an error message that tells the user they can run it with the help command.

```
.\program.exe start
```
This should fail because IdArgument should be required for the StartCommand.

## Notes
 - Need to use reflection to get the name of an argument, save it to a read-only Name property for ease of use if
 no name is specifically given.
 - ArgumentCollection a Dictionary but with helper properties to get sections of the table, like UnknownArguments, 
 CommandArguments, Switches, etc.
 - Even if the command line arguments are case insensitive, referring to the elements of the dictionary collection 
 are case sensitive.