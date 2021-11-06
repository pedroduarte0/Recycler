# Recycler
The idea to create Recycler came out of the realization that my Download folder tends to accumulate with an ever growing number of files and vast majority of the times they are totally useless. You can of course just delete the Download folder yourself manually periodically (or use a linux script ;) ), but I thought this would give a nice project to develop using test-driven development (TDD).

Recyler monitors previously selected folders and deletes files that go over a specified 'age'.

The concept of 'age' is not related to the date of the file itself, but meaning the time interval since a file was first detected by Recycler and the time that has passed so far.

As of the moment it is developed in C# using Visual Studio but the end goal is to become multi-platform (supporting Linux and MS Windows).

See wiki for more information.
