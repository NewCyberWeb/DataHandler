# NCW-DataHandler
New Cyber Web (NCW) - Datahandler project.
This repo is part of a series of repo's that form my NCW project, the goal of this project is to recreate the base elements the internet is based on.
Only for learning and entertainment (mostly myself) purposes.

## Project structure
This repo consists of two Visual Studio projects.\
The library project contains all the code for security and data handling.\
The second project is used to test the library and can be converted into a unit test project or eventually be removed.

## Technical information
Any important information on how certain segments of the library work are described here.

### Encryption
Too make sure everything is going to run smoothly the project is going to use an encryption handshake style
The encryption handshake starts off with Asymmetric RSA encryption, the client sends the server a request with his public key and a client secret.
The server can then respond with its own public key, a Symmetric private key and the hashed client secret.
If the client confirms that these values are legitimate, the client will send back one response that it has confirmed, this response is encrypted with the servers public key.\
Now bot parties know that the encryption handshake was successfull, the symmetric private key can be used to encrypt data.
On each cycle a new Initializeation Vector (IV) is send with the data,
