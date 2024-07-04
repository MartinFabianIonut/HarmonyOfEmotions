# Harmony of Emotions

**Introduction:**
The Harmony of Emotions application aims to seamlessly integrate technology with the ineffable world of classical music, providing a modern and relevant approach for contemporary audiences. Developed in C# under the .NET framework, the application leverages .NET Aspire orchestration to simplify configuration management and service interconnections, as well as to monitor the application's state during development.

In the following subsections, we will discuss the software product development lifecycle, from functionality descriptions (including requirements) to analysis, design (using appropriate diagrams), and implementation stages.

## Application Functionalities

The Harmony of Emotions application offers the following functionalities:

1. **Recommendation of Musical Compositions Based on Emotions:**
   - Using James Russel's bidimensional model (pleasure and arousal coordinates), users can select a specific emotion from this matrix. The application then provides a personalized list of five recommended music pieces. Recommendations are generated considering the distance from cluster centroids and proximity to other clusters, ensuring that the suggested pieces match the selected emotional state.
   ![Music Recommender System page](https://github.com/MartinFabianIonut/HarmonyOfEmotions/assets/118250277/23e9d9d9-4ba6-428b-9512-4163c66d412d)
![Music Recommender System - recommendations](https://github.com/MartinFabianIonut/HarmonyOfEmotions/assets/118250277/10809aeb-71d8-494a-bad9-6b34edc2bcae)



2. **Emotion Extraction from a Musical Composition:**
   - This feature allows users to examine the predictive model's behavior by searching for a musical composition on Spotify and providing it as input to the classification algorithm. The response will indicate one of four emotions: surprise, anger, happiness, or anxiety.
![Emotion recognition demo page](https://github.com/MartinFabianIonut/HarmonyOfEmotions/assets/118250277/840928a1-1e54-4acb-80c6-c4ffc22b54b4)

3. **Search for Musical Compositions:**
   - Users can search for music pieces based on keywords such as the title of a composition or the name of a composer or artist.
     ![Spotify Tracks page](https://github.com/MartinFabianIonut/HarmonyOfEmotions/assets/118250277/13fd3e69-f177-459d-9bfd-a1cabe4efe89)


4. **Finding the Most Representative Compositions of an Artist:**
   - As a result of the previous search, users might want to listen to other pieces composed/performed by a specific composer/artist. The application provides a list of top pieces by the respective artist.

5. **Finding Biographical Information About an Artist:**
   - To enrich the user's experience with the art world, the application includes short and relevant biographical information about artists/composers using APIs from Last.fm and MusicBrainz, in addition to Spotify, which does not provide such information.
     ![Artist Info page](https://github.com/MartinFabianIonut/HarmonyOfEmotions/assets/118250277/84fbfc4f-743a-4e28-9881-8d607b6bcf97)


6. **Marking Musical Preferences:**
   - Users can mark pieces of music as favorites or disliked after listening to searched or recommended passages.
     
     ![User Track Preferences page](https://github.com/MartinFabianIonut/HarmonyOfEmotions/assets/118250277/3231471c-3d27-4504-b4e4-a07c4c759ba1)

7. **System Authentication:**
   - Users must register and log in to use the described functionalities.


## Analysis and Design

In this section, we will explore the analysis and design process of the application's functionalities, starting with the system architecture from a conceptual perspective.

**Front-end:**

The Harmony of Emotions application's user interface is developed using Blazor, a modern web framework for front-end development using HTML, CSS, and C#. Blazor was chosen for several advantages, including the ability to write code in C#, use .NET libraries, inherent performance and security of .NET, and compatibility with various operating systems and hosting platforms.

To enable communication between Razor pages and the API, several services are configured in Program.cs, including:

- **Authentication Services:**
  - AuthService and TokenService manage authentication and token storage.
  
- **Token Access:**
  - BearerTokenHandler intercepts all HTTP calls, adding the necessary authentication token.
  
- **HTTP Calls:**
  - ApiClient is configured for HTTP calls to the application's API, specifying JSON for data exchange. Responsible classes include ArtistInfoService, SpotifyTrackService, MusicRecommenderSystemService, and UserTrackPreferencesService.

**Back-end:**

Creating a robust, scalable, reliable, and efficient Web API service is crucial. The application follows the Model-View-Controller (MVC) architecture to ensure separation of concerns and facilitate maintenance and future extensions. Key controller classes include:

- **ArtistInfoController:**
  - Manages requests related to artists' biographical information by interacting with LastFmService and MusicBrainzService.

- **SpotifyTrackController:**
  - Handles interactions with Spotify Web API for searching and retrieving information about musical compositions.

- **UserTrackPreferencesController:**
  - Manages user preferences, storing and retrieving data from Azure SQL Database.

- **MusicRecommenderSystemController:**
  - Provides musical recommendations based on users' selected emotions using RecommenderService.

Key services include:

- **ArtistService:** 
  - Defines the main schema for obtaining artist data.

- **LastFmService:** 
  - Communicates with Last.fm API to retrieve biographical information.

- **MusicBrainzService:** 
  - Interacts with MusicBrainz API to obtain additional artist data.

- **FirebaseRepository:** 
  - Manages data stored in Firebase, specifically the static information of the dataset.

- **RecommenderService:** 
  - Contains logic for generating musical recommendations and also finding the emotions for a specified track (from an Url).

- **SpotifyService:** 
  - Interacts with Spotify Web API for retrieving popular tracks of an artist and searching the Spotify music library by keywords.

- **UserTrackService:** 
  - Manages users' musical preferences.

**Design Patterns:**

The use of design patterns is essential for a clear application structure and efficient implementation. Key design patterns used include:

1. **Dependency Injection:**
   - Used for utilizing abstractions defined in the project by injecting dependencies in the constructor. Implemented using C# 12's new syntax for primary constructors.

2. **Model-View-Controller (MVC):**
   - Provides modularity, flexibility, reusability, and scalability, used as the framework base for developing the Web API.

3. **Singleton:**
   - Ensures a single instance of a type is created and used throughout the application's runtime.

4. **Decorator:**
   - Adds responsibilities to individual objects dynamically and transparently, used for setting HTTP client headers in both API and web client projects.

## Implementation

Thanks to .NET Aspire, orchestrating the application is straightforward using service discovery by reference. This involves registering service projects in a .NET Aspire AppHost project. 

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder
    .AddProject<Projects.HarmonyOfEmotions_ApiService>("harmony-of-emotions-apiservice");

builder.AddProject<Projects.HarmonyOfEmotions_Client>("harmony-of-emotions-client")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);
```
To enable the web application to communicate with the API service via HTTP, specifying the endpoint name in the host portion of the HTTP request URI is sufficient. For the ApiClient service in the front-end application, set:

```csharp
client.BaseAddress = new Uri("https+http://harmony-of-emotions-apiservice");
```

Moreover, monitoring the state of both projects is straightforward due to the dedicated interface of .NET Aspire. It can be observed that both HTTP and HTTPS endpoints have been created, as specified in the base address of the HTTP client mentioned earlier. Among the functionalities of the orchestrator, we highlight:
- Structured log monitoring
- Tracing and performance metrics at runtime (both for HTTP calls and the resources consumed by concurrently running processes).


### Technologies/Frameworks Used

Most of the technologies used in the development of the software product have already been mentioned, so the list below serves as a brief recap of the most important ones:

1. **ML.NET:** A framework developed by Microsoft for integrating machine learning capabilities into .NET. It is open-source and multi-platform [Mic24b], achieving very good performance compared to scikit-learn or H2O.

2. **ASP.NET Core Identity & Entity Framework Core:** Part of ASP.NET Core, this framework offers authentication and authorization functionalities for web applications. Additionally, it can auto-generate tables to maintain information about user accounts and even provide implicit endpoints for operations on database entries, such as registration, login, email confirmation, password reset, and two-factor authorization management.

3. **ASP.NET Core Blazor:** The .NET framework used for building the user interface [MLRo24].

4. **.NET Aspire:** A framework developed for orchestrating .NET applications, providing key services such as log monitoring, runtime performance metrics, endpoint state inspection, service discovery based on name, and proper HTTP client configuration.
