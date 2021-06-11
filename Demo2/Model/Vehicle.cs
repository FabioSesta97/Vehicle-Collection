using System;
using System.Text.Json.Serialization;

namespace Demo2
{
    public abstract class Vehicle
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("Maker")]
        public string Maker { get; set; }

        [JsonPropertyName("Plate")]
        public string Plate { get; set; }

        public abstract void Sound();

    }

    public class Car : Vehicle
    {
        [JsonPropertyName("Seats")]
        public int Seats { get; set; }

        public Car(int id, string maker, string plate, int seats)
        {
            ID = id;
            Maker = maker;
            Plate = plate;
            Seats = seats;
        }

        public Car()
        {
        }

        public override string ToString()
        {
            return $"Car | ID: {ID} - Maker: {Maker} - Plate: {Plate} - Seats: {Seats}";
        }

        public override void Sound()
        {
            Console.WriteLine("**CLACSON**");
        }
    }

    public class Truck : Vehicle
    {
        [JsonPropertyName("Axes")]
        public int Axes { get; set; }

        public Truck(int id, string maker, string plate, int axes)
        {
            ID = id;
            Maker = maker;
            Plate = plate;
            Axes = axes;
        }

        public Truck()
        {
        }

        public override string ToString()
        {
            return $"Truck | ID: {ID} - Maker: {Maker} - Plate: {Plate} - Axes: {Axes}";
        }

        public override void Sound()
        {
            Console.WriteLine("**TRUMPET**");
        }
    }

    public class Bike : Vehicle
    {
        [JsonPropertyName("Gears")]
        public int Gears { get; set; }

        public Bike(int id, string maker, string plate, int gears)
        {
            ID = id;
            Maker = maker;
            Plate = plate;
            Gears = gears;
        }
        public Bike()
        {
        }

        public override string ToString()
        {
            return $"Bike | ID: {ID} - Maker: {Maker} - Plate: {Plate} - Gears: {Gears}";
        }

        public override void Sound()
        {
            Console.WriteLine("**BELL**");
        }
    }
}