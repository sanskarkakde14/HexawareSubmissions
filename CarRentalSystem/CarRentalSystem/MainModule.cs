
using CarRentalSystem.Model;
using CarRentalSystem.Repository;
using CarRentalSystem.Exceptions;
using System.Transactions;

namespace CarRentalSystem
{
    public class MainModule
    {
        private readonly ICarLeaseRepository repository;
        public MainModule(ICarLeaseRepository repository)
        {
            this.repository = repository;
        }

        public void Run()
        {
            bool exitProgram = false;

            while (!exitProgram)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                try
                {
                    ProcessUserChoice(choice, ref exitProgram);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        private void ProcessUserChoice(string choice, ref bool exitProgram)
        {
            switch (choice)
            {
                case "1":
                    AddNewCar();
                    break;
                case "2":
                    ListAvailableCars();
                    break;
                case "3":
                    AddNewCustomer();
                    break;
                case "4":
                    ListCustomers();
                    break;
                case "5":
                    CreateNewLease();
                    break;
                case "6":
                    ReturnCar();
                    break;
                case "7":
                    ViewLeaseHistory();
                    break;
                case "8":
                    ListRentedCars();
                    break;
                case "9":
                    RemoveCar();
                    break;
                case "10":
                    RemoveCustomer();
                    break;
                case "11":
                    ViewActiveLeases();
                    break;
                case "12":
                    exitProgram = true;
                    Console.WriteLine("Thank you for using the Car Rental System. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        public void DisplayMenu()
        {
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("Car Rental System");
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("1. Add new car");
            Console.WriteLine("2. List available cars");
            Console.WriteLine("3. Add new customer");
            Console.WriteLine("4. List customers");
            Console.WriteLine("5. Create new lease");
            Console.WriteLine("6. Return car");
            Console.WriteLine("7. View lease history");
            Console.WriteLine("8. List rented cars");
            Console.WriteLine("9. Remove a car");
            Console.WriteLine("10. Remove a customer");
            Console.WriteLine("11. View active leases");
            Console.WriteLine("12. Exit");
            Console.WriteLine(new string('-', 40));
            Console.Write("Enter your choice: ");
        }

        public void AddNewCar()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("Add New Car");
            Console.WriteLine(new string('=', 40));
            Console.Write("Enter make: ");
            string make = Console.ReadLine();
            Console.Write("Enter model: ");
            string model = Console.ReadLine();
            Console.Write("Enter year: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Enter daily rate: ");
            decimal dailyRate = decimal.Parse(Console.ReadLine());
            Console.Write("Enter passenger capacity: ");
            int passengerCapacity = int.Parse(Console.ReadLine());
            Console.Write("Enter engine capacity: ");
            string engineCapacity = Console.ReadLine();
            Vehicle newCar = new Vehicle(0, make, model, year, dailyRate, "available", passengerCapacity, engineCapacity);
            repository.AddCar(newCar);
            Console.WriteLine("\nNew car added successfully:");
            Console.WriteLine($"Make: {make}, Model: {model}, Year: {year}");
            Console.WriteLine($"Daily Rate: {dailyRate:C}, Capacity: {passengerCapacity}, Engine: {engineCapacity}");
        }

        public void ListAvailableCars()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("Available Cars");
            Console.WriteLine(new string('=', 40));
            List<Vehicle> availableCars = repository.ListAvailableCars();
            if (availableCars.Any())
            {
                Console.WriteLine(new string('-', 85));
                Console.WriteLine($"{"ID",-5}{"Make",-15}{"Model",-15}{"Year",-6}{"Daily Rate",-15}{"Capacity",-10}{"Engine",-15}");
                Console.WriteLine(new string('-', 85));

                foreach (var car in availableCars)
                {
                    Console.WriteLine($"{car.VehicleID,-5}{car.Make,-15}{car.Model,-15}{car.Year,-6}{car.DailyRate,15:C2}{car.PassengerCapacity,10}{car.EngineCapacity,-15}");
                }
                Console.WriteLine(new string('-', 85));
            }
            else
            {
                Console.WriteLine("No available cars at the moment.");
            }
        }

        public void AddNewCustomer()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("👤 Add New Customer");
            Console.WriteLine(new string('=', 40));
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            Console.Write("Enter phone number: ");
            string phoneNumber = Console.ReadLine();
            Customer newCustomer = new Customer(0, firstName, lastName, email, phoneNumber);
            repository.AddCustomer(newCustomer);
            Console.WriteLine("\nNew customer added successfully:");
            Console.WriteLine($"Name: {firstName} {lastName}");
            Console.WriteLine($"Email: {email}, Phone: {phoneNumber}");
        }

        public void ListCustomers()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("👥 Customer List");
            Console.WriteLine(new string('=', 40));
            List<Customer> customers = repository.ListCustomers();
            if (customers.Any())
            {
                Console.WriteLine(new string('-', 85));
                Console.WriteLine($"{"ID",-5}{"Name",-30}{"Email",-25}{"Phone",-20}");
                Console.WriteLine(new string('-', 85));
                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.CustomerID,-5}{customer.FirstName + " " + customer.LastName,-30}{customer.Email,-25}{customer.PhoneNumber,-20}");
                }
                Console.WriteLine(new string('-', 85));
            }
            else
            {
                Console.WriteLine("No customers found in the system.");
            }
        }

        public void CreateNewLease()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("Create New Lease");
            Console.WriteLine(new string('=', 40));
            try
            {
                // Gather lease information
                Console.Write("Enter customer ID: ");
                int customerId = int.Parse(Console.ReadLine());
                Console.Write("Enter car ID: ");
                int carId = int.Parse(Console.ReadLine());
                Console.Write("Enter start date (yyyy-mm-dd): ");
                DateTime startDate = DateTime.Parse(Console.ReadLine());
                Console.Write("Enter end date (yyyy-mm-dd): ");
                DateTime endDate = DateTime.Parse(Console.ReadLine());

                // Calculate lease amount
                Vehicle car = repository.FindCarById(carId);
                if (car == null)
                {
                    throw new CarNotFoundException("Vehicle Not Found");
                }
                int numberOfDays = (int)(endDate - startDate).TotalDays + 1;
                decimal totalAmount = car.DailyRate * numberOfDays;

                Console.WriteLine($"\nTotal lease amount: ₹{totalAmount:F2}");

                // Get payment method
                Console.WriteLine("Select payment method:");
                Console.WriteLine("1. UPI");
                Console.WriteLine("2. Debit Card");
                Console.WriteLine("3. Net Banking");
                Console.Write("Enter your choice (1-3): ");
                int paymentChoice = int.Parse(Console.ReadLine());
                string paymentMethod = paymentChoice switch
                {
                    1 => "UPI",
                    2 => "Debit Card",
                    3 => "Net Banking",
                    _ => throw new ArgumentException("Invalid payment method selected.")
                };

                // Display lease details and confirm payment
                Console.WriteLine("\nLease Details:");
                Console.WriteLine($"Customer ID: {customerId}");
                Console.WriteLine($"Car ID: {carId}");
                Console.WriteLine($"Start Date: {startDate:d}");
                Console.WriteLine($"End Date: {endDate:d}");
                Console.WriteLine($"Total Amount: ₹{totalAmount:F2}");
                Console.WriteLine($"Payment Method: {paymentMethod}");

                Console.Write("\nConfirm payment and create lease? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() != "Y")
                {
                    Console.WriteLine("Lease creation cancelled.");
                    return;
                }

                // Create lease and payment records
                using (var transaction = new TransactionScope())
                {
                    Lease newLease = repository.CreateLease(customerId, carId, startDate, endDate);
                    repository.RecordPayment(newLease.LeaseID, totalAmount);

                    transaction.Complete();

                    // Display confirmation
                    Console.WriteLine("\nLease and payment created successfully:");
                    Console.WriteLine($"Lease ID: {newLease.LeaseID}");
                    Console.WriteLine($"Customer ID: {customerId}");
                    Console.WriteLine($"Car ID: {carId}");
                    Console.WriteLine($"Start Date: {startDate:d}");
                    Console.WriteLine($"End Date: {endDate:d}");
                    Console.WriteLine($"Lease Type: {newLease.Type}");
                    Console.WriteLine($"Total Amount: ₹{totalAmount:F2}");
                    Console.WriteLine($"Payment Method: {paymentMethod}");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input format. Please enter valid numbers and dates.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nError creating lease: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
            }
        }


        public void ReturnCar()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("Return Car");
            Console.WriteLine(new string('=', 40));
            Console.Write("Enter lease ID: ");
            int leaseId = int.Parse(Console.ReadLine());
            repository.ReturnCar(leaseId);
            Console.WriteLine("\nCar returned successfully.");
            Console.WriteLine($"Lease ID: {leaseId} has been closed.");
        }

        public void ViewLeaseHistory()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("Lease History");
            Console.WriteLine(new string('=', 40));

            List<Lease> leaseHistory = repository.ListLeaseHistory();
            if (leaseHistory.Any())
            {
                Console.WriteLine(new string('-', 120));
                Console.WriteLine($"{"LeaseID",-8}{"CustomerID",-12}{"CarID",-8}{"Start Date",-15}{"End Date",-15}{"Total Amount",-15}");
                Console.WriteLine(new string('-', 120));

                foreach (var lease in leaseHistory)
                {
                    // Fetch the vehicle details using vehicleID
                    Vehicle vehicle = repository.FindCarById(lease.vehicleID);
                    if (vehicle == null)
                    {
                        Console.WriteLine($"Vehicle not found for Lease ID {lease.LeaseID}");
                        continue;  // Skip this lease if vehicle details are missing
                    }

                    // Calculate total amount based on the lease duration and daily rate
                    int numberOfDays = (int)(lease.EndDate - lease.StartDate).TotalDays + 1;  // +1 to include both start and end days
                    decimal totalAmount = vehicle.DailyRate * numberOfDays;

                    // Display the lease details with the calculated total amount
                    Console.WriteLine($"{lease.LeaseID,-8}{lease.CustomerID,-12}{lease.vehicleID,-8}{lease.StartDate.ToString("yyyy-MM-dd"),-15}{lease.EndDate.ToString("yyyy-MM-dd"),-15}{totalAmount,15:C2}");
                }

                Console.WriteLine(new string('-', 120));
            }
            else
            {
                Console.WriteLine("No lease history found.");
            }
        }

        public void ListRentedCars()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("Rented Cars");
            Console.WriteLine(new string('=', 40));
            List<Vehicle> rentedCars = repository.ListRentedCars();
            if (rentedCars.Any())
            {
                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"{"ID",-5}{"Make",-15}{"Model",-15}{"Year",-10}{"Daily Rate",-15}{"Capacity",-10}{"Engine",-10}");
                Console.WriteLine(new string('-', 80));

                foreach (var car in rentedCars)
                {
                    Console.WriteLine($"{car.VehicleID,-5}{car.Make,-15}{car.Model,-15}{car.Year,-10}{car.DailyRate:C}{car.PassengerCapacity,-10}{car.EngineCapacity,-10}");
                }
                Console.WriteLine(new string('-', 80));
            }
            else
            {
                Console.WriteLine("No cars are currently rented.");
            }
        }

        public void RemoveCar()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("Remove Car");
            Console.WriteLine(new string('=', 40));
            Console.Write("Enter car ID: ");

            try
            {
                int carId = int.Parse(Console.ReadLine());
                repository.RemoveCar(carId); // Attempt to remove the car

                Console.WriteLine("\nCar removed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
            }
            catch (Exception ex) // General exception handling
            {
                Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
            }
        }


        public void RemoveCustomer()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("Remove Customer");
            Console.WriteLine(new string('=', 40));
            Console.Write("Enter customer ID: ");

            try
            {
                int customerId = int.Parse(Console.ReadLine());
                repository.RemoveCustomer(customerId); // Attempt to remove the customer

                Console.WriteLine("\nCustomer removed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
            }
            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
            }
            catch (Exception ex) // General exception handling
            {
                Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
            }
        }


        public void ViewActiveLeases()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("Active Leases");
            Console.WriteLine(new string('=', 40));
            List<Lease> activeLeases = repository.ListActiveLeases();
            if (activeLeases.Any())
            {
                Console.WriteLine(new string('-', 100));
                Console.WriteLine($"{"LeaseID",-8}{"CustomerID",-12}{"CarID",-8}{"Start Date",-15}{"End Date",-15}{"Total Amount",-15}");
                Console.WriteLine(new string('-', 100));
                foreach (var lease in activeLeases)
                {
                    Console.WriteLine($"{lease.LeaseID,-8}{lease.CustomerID,-12}{lease.vehicleID,-8}{lease.StartDate.ToString("yyyy-MM-dd"),-15}{lease.EndDate.ToString("yyyy-MM-dd"),-15}");
                }
                Console.WriteLine(new string('-', 100));
            }
            else
            {
                Console.WriteLine("No active leases found.");
            }
        }


    }
}