# Zadanie

Poniższe zadanie wymaga napisania minimum kodu aplikacji (back-end code, API), bez interfejsu użytkownika. Programista musi jednak odpowiednio sprawdzić kod, aby w komfortowy sposób być pewnym poprawności jego działania. Zadanie należy wykonać w C#.

Zaimplementuj podstawowe funkcjonalności umożliwiające zarządzanie flotą statków. Zakładamy, że każdy statek ma swój globalny unikalny numer składający się z 7 cyfr, tzw. IMO number - Wikipedia, np. 9074729. Dodatkowo statek ma swoją nazwę jak „Black Pearl”, długość, szerokość. Funkcjonalność programu powinna pozwolić na dodawanie statków do rejestru, zarejestrowanie nowego statku musi wiązać się również z walidacją danych – długość nie może być ujemna, a numer IMO powinien być sprawdzony pod kątem checksumy – sprawdź dokumentację w podanym linku do Wikipedii. Nr IMO jest unikalny, w rejestrze nie powinny pojawić się duplikaty tych numerów. Zakładamy dwa typy statków

- pasażerski (Passenger Ship) - zawiera dodatkowo listę pasażerów (pasażerowie to nie załoga, załoga występuje w obu statkach, ale pomijamy załogę w wymaganiach)

- tankowiec (Tanker Ship) – pozwala na zatankowanie różnego typu paliwa do stale zamontowanych na statku zbiorników. Liczba tych zbiorników może być różna na każdym statku, każdy zbiornik ma swoją pojemność w litrach. Każdy ze zbiorników może być napełniony paliwem jednego rodzaju do jego maksymalnej pojemności – zakładamy dwa rodzaje paliwa, Diesel oraz Heavy Fuel. Funkcjonalność programu ma pozwalać na:

- dodawanie statków każdego typu do rejestru statków

- aktualizacje listy pasażerów dla statku pasażerskiego

- zatankowanie danego zbiornika tankowca podaną ilością paliwa

- opróżnienie danego zbiornika

