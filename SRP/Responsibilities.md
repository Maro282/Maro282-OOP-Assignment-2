### Task 1.1

##### 1- AppointmentDesk
	. Trying to book an appointment
	. Check that the appointment in working hours or not
	. Find nearest available appointment through specified hours
	. Save appointment to calender
	. Send reminder MSG
	. Allign time to fit slot Minutes
	
 The class has different sources to modify in the class so it's low cohesion if we need to modify the reminder notification to be done with any other application we will have to modify in the class which not responsible for that action so we have to separate unrelated action in other class.


#### 2- KitchenTicket
	. Adding items to Kitchen system
	. Detect Allergens based on ingredients
	. Estimates time for order to be ready  
	. Detect which lane should handle the order 
	. create text that will be printed
	
the problem for having that distinct responsibilities is when we had to modify printing format ? we will have to modify in the kitchen system class itself . also if we need to change calculation equation for ETA we also need to modify in the order items details class so we have to separate unrelated behaviors from each other to have the same reason to change in every class 


#### 3- GradeBook
	. Assign student scores
	. Calculate Student grades Average
	. Returning grade letter based on avg
	. Check student meet honor roll or not
	. Print student details
	. Save to csv file

why involving file exporting and printing format in the business rule class of calculating and checking student grades ? it's not a logic at all so we had to separate that in individual classes


#### 4 - CourseEnrollmentDesk
	. Register Student
	. Return student waiting list position
	. Welcome registered student
	. Promoting from waiting list
	. Calculate course Tuition

if the accounting team askes us to modify something in the calculation equation we will had to update in the main class , if the marketing team askes us to update welcoming message format we will had to change in the main class 

#### 5 - CheckoutBasket
	. Adding new line to basket
	. Applying copoun and enabling Gift Wrapping
	. Calculating discount amount 
	. Calculating grand total
	. Making gift message card
	. Formatting payload strings authorization codes for card transactions.

changing coupon rules shouldn't require modifying payment authorization or gift message formatting.

#### 6 - WardBoard
	. Assign patient to bed
	. Caculating acuity
	. creating hand off representation 
	. save pager event
	. Exporting CSV 

Each responsibility can change for a different reason. For example, changing clinical scoring rules shouldn't require modifying CSV export or nurse note formatting.

#### 7 - LoanDesk
	. Calculate risk score
	. Check eligability
	. Check required documents
	. Creating decisional letter
	. Exporting CSV File

, changing the required documents shouldn't require modifying the risk calculation or decision letter generation.


#### 8 - SubscriptionBilling
	. Calculate how much should user pay 
	. Telling next invoice number
	. register FailedPayment
	. Creates an email for a customer who failed paymen
	. Create Ledger Journal Line


changing the invoice number scheme shouldn't require modifying the proration logic or email content.

#### 9 - SupportTicket
	. Saving Ticket Info
	. Check Priority
	. Append Customer new message
	. Calculate SLA
	. Check if deadline is breached or not
	. Create public reply
	. Internal Scalation


the main class should responsible for saving ticket data and checking the priority of the ticket once it created and append user message only . so calculating the SLA , public reply format  on user message and escalation process message format all of mentioned is away from the main responsibility of the main class


#### 10 - WarehousePickList
	. Determine needed product with it's details
	. Return list of products with actuall quantity can be picked
	. Determine where the worker should walk and in what order
	. Creating a script for worker to read
	. Create XML batch

Combining product requirement tracking, stock allocation, picker instruction generation, and XML export makes the system harder to update because warehouse rules, worker workflows, and external system formats evolve independently. For example, changing the warehouse layout shouldn't require modifying the XML integration or stock allocation logic.