# Druck Pace 1001 Example in C#
Below is an example on connecting and recieving pressure data from a Druck Pace 1001 over ethernet, in C#.

## Libraries / References
The Ivi.Visa reference is used
```
using Ivi.Visa;
```
The reference can be imported into your C# project after installing the NI-VISA drivers, which includes the library needed. This can be downloaded from [here](https://www.ni.com/en/support/downloads/drivers/download.ni-visa.html#494653).

## Example Code
Change the below GlobalResourceManager.Open to the IP Address of your Druck Pace 1001. The code returns the pressure as a string method PressuremBar().
``` 
public string PressuremBar()
        {
            // Connect to the druck using an IVisaSession and globalresourcemanager
            using (IVisaSession res = GlobalResourceManager.Open("TCPIP::10.115.46.84::inst0::INSTR", AccessModes.ExclusiveLock, 2000))
            {
                //if the resource is a IMessageBasedSession (which it should be)
                if (res is IMessageBasedSession session)
                {
                    // Ensure termination character is enabled as here in example we use a SOCKET connection.
                    session.TerminationCharacterEnabled = true;
                    // Request information about the druck.
                    session.FormattedIO.WriteLine(":SENSe:PRESsure?");
                    //Capture the information
                    string idn = session.FormattedIO.ReadLine();
                    //Set the index of the payload
                    int index = idn.IndexOf(' ');
                    //only get these specific characters, we're only interested in the numbers.
                    string pressure = idn.Substring(index + 1, 7);
                    //Return the pressure
                    return pressure;
                }
                else
                {
                    return "Offline";
                }
            }
        }
```

and that's it! The C# method returns the pressure as a string. Call the method in the main body of your code.
