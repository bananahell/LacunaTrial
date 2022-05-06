# Lacuna's Job Trial
Pedro Nogueira  
Brasília - May 2022  

Trial created by [Lacuna](https://www.lacunasoftware.com/en/) to test potential candidates for their recruitment process in C#.  
The suggested challenge involves managing a DNA's strand as a string of nucleobases A T C G. Instructions are present in their [README](https://gene.lacuna.cc/).  
Fun personal fact: I have already worked with the Unity engine in C#, but I never ACTUALLY used dotnet before this project. Of course I want the job, but I find that the experience in itself was already pretty great.  

## HTTP communications in C#

Firstly, the candidate needs to create a user within the website's database using a username, an email, and a password.  
This user entry is necessary to then retrieve an access token from the website.  
Finally, this access token is what enables the candidate to retrieve 1 of 3 jobs proposed in this challenge.  
All of these queries are made by using HTTP requests directly to https://gene.lacuna.cc/ within its directories. The only methods used were GET and POST. My code has all of these requests made inside the HttpServices.cs file in the root of the project.  

## The jobs in this challenge

Using the access token previously acquired, the candidate can now request for a random job among 3 options. I implemented all of 3 of them in the BiologyDepartment.cs file in the root of the project, being called whenever they are necessary for the job demanded. Those 3 options are:  

### Decode strand

The decode strand job gives the candidate a string in base 64 format and asks back for a string containing the nucleobases A T C G.  

### Encode strand

The encode strand job gives the candidate a string composed by the nucleobases A T C G and asks back for a string encoded in base 64 format.  

### Check gene

The check gene job sends a strand of DNA and a gene, both encoded in base 64, and asks the candidate if the gene is activated in the strand or not. The conditions for a gene being activated in a DNA strand is having more than 50% of its nucleobases in order present in the DNA strand.  

## The result

Each of these jobs return information in the form of a json object containing whether if the job was answered succesfully or not. All of my trials were succesfull. Now is for hoping that I actually get the job. I'm starving.  
