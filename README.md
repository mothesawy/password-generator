# **Password Generator**

A tool to generate passwords using hashing and UUIDv5, written in C#!
##### THIS TOOL DOES NOT SAVE OR SEND ANY DATA OUT OF YOUR COMPUTER. CHECK THE SOURCE CODE TO MAKE SURE!

## features:
- This tool can use both relative paths and absolute paths.
- This tool does not save your password any where. give it the same account names and secret key and it will generate the same password every time.
- Generate password for one platform using the prompt.
- Batch generate passwords for multiple platforms and accounts using cli arguments.

## Installation:
- Download binaries from _Releases_ page or clone and build it yourself.
- You can add the binary to your path to run it from anywhere.
- This tool __does not__ require _.net core_ to be installed. It's self contained.
- Run `pw-gen --help` for more info.

## Usage:
- Usage 1: 
    - Run `pw-gen` and follow prompt instructions!
    - **inputs:**
        - username
        - app/platform name
        - master secret key
        - password length

- Usage 2:
    - Prepare a file containing your user names and apps using the following format and save it as txt:
    - ```
        user1@app1
        user2@app2
        user3@app3
      ```
    - Example: `rvpx367@github`
    - Run the tool with `--file` or `-f` flag followed by the the path of the file containing the accounts.
    - Run the tool with `--length` or `-l` flag followed by length of the passwords (optional - default=16, min=8, max=32)
    - Example: `pw-gen -f "path/to/your/file.txt"`
    - Example: `pw-gen -f "./file.txt" --length 12`
    - Then enter the master secret key to generate passwords.
    - A file with the name "pwgen_passwords.txt" will be generated containing the passwords in the same place as your file.

## TODO:
- [X] Add password length feature
- [X] Add generate passwords for multiple platforms at once
- [ ] Cross-Platform GUI
