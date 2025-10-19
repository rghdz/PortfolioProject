“Bli en Avenger, slutför uppdrag med Jarvis, samla poäng och få nya AI-genererade missions!”
--------------------------------------------------------------------------------------------------------

Avengers Hero System -
Registrera dig som en Avenger och börja lösa uppdrag tillsammans med Jarvis. Slutför missions, 
samla poäng och få AI-genererade nya uppdrag som matchar din hjälteroll. Alla Jarvis-meddelanden 
visas i rött, och du kan lägga till egna anteckningar och få email-notiser om nya missions.
-------------------------------------------------------------------------------------------------------

Snabb Översikt -
Det här programmet låter användare registrera sig som Avengers och börja med ett standardmission 
specifikt för deras roll. Användaren kan logga in, se sina missions, markera dem som slutförda och 
få poäng baserat på uppdragets prioritet. När missions slutförs kan Jarvis generera nya AI-baserade 
missions som passar hjälten. Systemet låter även användaren lägga till egna anteckningar och får 
email-notiser om nya missions. Alla meddelanden från Jarvis visas i rött i konsolen för tydlig 
AI-kommunikation. Programmet kombinerar användarregistrering, missions, poängsystem, AI-integration 
och notifieringar i ett interaktivt och objektorienterat C#-system.
-------------------------------------------------------------------------------------------------------

Detaljerad beskrivning av programmet - 
Avengers Hero System är ett interaktivt C#-program där användare kan registrera sig som olika Avengers 
och delta i uppdrag (missions) som Jarvis genererar. När en användare registrerar sig analyserar Jarvis 
deras personlighet genom några frågor och föreslår en passande Avenger-roll. Under registreringen får 
användaren också välja ett starkt lösenord, verifiera sitt telefonnummer via tvåfaktorsautentisering (2FA) 
med Twilio och ange en e-postadress för att ta emot notifieringar om nya missions.

Varje Avenger börjar med ett default mission som är specifikt för deras roll. Missions innehåller information
som titel, beskrivning, deadline, prioritet (Low, Medium, High) och status om det är slutfört eller inte. 
Användaren kan visa sitt aktuella mission, se alla missions i systemet, markera mission som slutförda, 
samt lägga till egna anteckningar eller påminnelser för varje mission.

När ett mission markeras som slutfört får användaren poäng baserat på uppdragets prioritet:
High ger 10 poäng, Medium 5 poäng och Low 2 poäng. Efter att ett mission är slutfört frågar Jarvis 
användaren om de vill generera nya AI-skapade missions. Om användaren tackar ja kan Jarvis skapa 
1–5 nya missions som passar deras Avenger-roll. Dessa nya missions får samma funktionalitet som default-missions, 
dvs. användaren kan markera dem som completed, byta status från NO till YES, samt lägga till egna anteckningar.

Alla meddelanden från Jarvis, inklusive instruktioner, uppmaningar och AI-genererade svar, skrivs ut i rött 
för att tydligt skilja AI-kommunikationen från övriga konsolutskrifter. Systemet använder en JarvisHelper-klass 
för att hantera detta konsekvent över hela programmet. AI-integrationen hanteras via OpenAI, och systemet kan 
även skicka email-notiser till användaren när nya missions genereras, via JarvisNotifier.

Programmet är uppbyggt med flera klasser:
	•	AvengersProfile hanterar användare, registrering, lösenord, 2FA och inloggning.
	•	MissionManagement hanterar skapande, visning, uppdatering, slutförande och poängsystem för missions.
	•	JarvisChat kommunicerar med OpenAI för att generera kreativa missions och svara på användarens frågor.
	•	JarvisRoleMatcher avgör Avenger-roll baserat på personlighetsfrågor.
	•	JarvisMissionFinder genererar AI-baserade missions.
	•	JarvisNotifier skickar email-notifieringar.
	•	JarvisHelper säkerställer att alla Jarvis-meddelanden visas i rött.

Huvudprogrammet (Program) består av en asynkron Main-metod som kör huvudloopen. Den visar först en startmeny 
där användaren kan registrera sig, logga in eller avsluta programmet. Efter inloggning visas användarens meny 
med alternativ för att lägga till missions, visa missions, slutföra missions, uppdatera missions och logga ut. 
Asynkroniteten (async Task Main) möjliggör att AI-anrop och email kan ske utan att blockera programmet, 
vilket gör interaktionen snabb och smidig.

Sammanfattningsvis kombinerar systemet objektorienterad programmering, AI-integration, användarinteraktivitet, 
missions och poängsystem med notifieringar, och skapar en dynamisk och engagerande upplevelse där användare kan 
känna sig som riktiga Avengers som löser uppdrag tillsammans med Jarvis.
