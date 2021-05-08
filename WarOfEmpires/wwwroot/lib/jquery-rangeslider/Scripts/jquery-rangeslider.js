(function ($) {

    // Extension for creating range slider; supports multiple creations in one call
    $.fn.rangeslider = function (settings, callback) {
        // Allow callback to be the only argument
        if ($.isFunction(settings)) {
            callback = settings;
            settings = null;
        }

        return $(this).each(function () {
            // Get object from data
            let rangeslider = $(this).data('rangeslider');

            if (!rangeslider) {
                // Create object
                let options = $.extend({}, $.fn.rangeslider.defaults, settings);
                rangeslider = new Rangeslider($(this), options);

                // Add object to data
                $(this).data('rangeslider', rangeslider);
            }

            // Call the callback, bound to the rangeslider
            if ($.isFunction(callback)) {
                callback.bind(rangeslider)(rangeslider);
            }
        });
    }

    // Set defaults for extension
    $.fn.rangeslider.defaults = {
        // The starting point of the value range
        // Defaults to the data-property range-start with as fallback 0
        getRangeStart: function (element) {
            let rangeStart = parseInt($(element).data('range-start'));

            if (isNaN(rangeStart)) {
                rangeStart = 0;
            }

            return rangeStart;
        },
        // The number of steps
        // The number of possible values is step count + 1
        // Defaults to the data-property step-count with as fallback 10
        getStepCount: function (element) {
            let stepCount = parseInt($(element).data('step-count'));

            if (isNaN(stepCount) || stepCount <= 0) {
                stepCount = 10;
            }

            return stepCount;
        },
        // The value increase for each step
        // The range end value will be range start + step count * step size
        // Defaults to the data-property step-size with as fallback 10
        getStepSize: function (element) {
            let stepSize = parseInt($(element).data('step-size'));

            if (isNaN(stepSize) || stepSize <= 0) {
                stepSize = 10;
            }

            return stepSize;
        },
        // The value when initializing the range slider
        // Values outside the range of valid values will be corrected when initializing
        // Defaults to the data-property value with as fallback 0
        getValue: function (element) {
            let value = parseInt($(element).data('value'));

            if (isNaN(value)) {
                value = 0;
            }

            return value;
        },
        // The field name to use for the generated hidden input field
        // Defaults to the data-property field-name
        getFieldName: function (element) {
            return $(element).data('field-name');
        },
        // The input is the hidden input field that contains the current value
        // It always gets at least the class 'rangeslider-input', type 'hidden' and name as specifified by data-property field-name
        getInputAttributes: function () {
            return {};
        },
        // The thumb is the element that drags
        // It always gets at least the class 'rangeslider-thumb'
        getThumbAttributes: function () {
            return {};
        },
        // The track is the background element along which the thumb drags
        // It always gets at least the class 'rangeslider-track'
        getTrackAttributes: function () {
            return {};
        }
    }

    // Rangeslider implementation
    function Rangeslider(element, options) {
        this.element = element;
        this.options = options;

        this.rangeStart = options.getRangeStart(this.element);
        this.stepCount = options.getStepCount(this.element);
        this.stepSize = options.getStepSize(this.element);
        this.dragStatus = {
            dragging: false
        };

        this.element.addClass('rangeslider');
        this.element.children().hide();

        this.thumb = this.createElement('<div>', 'rangeslider-thumb', this.options.getThumbAttributes());
        this.track = this.createElement('<div>', 'rangeslider-track', this.options.getTrackAttributes());
        this.input = this.createElement('<input>', 'rangeslider-input', this.options.getInputAttributes(), {
            name: this.options.getFieldName(this.element),
            type: 'hidden'
        })
        this.element.append(this.track, this.thumb, this.input);

        // Set value only after elements are in place
        this.setValue(options.getValue(this.element));

        // Event handlers
        this.element.click(this, eventHandlers.click);
        this.thumb.on('mousedown', this, eventHandlers.thumbDragStart);
        $(document).on('mousemove', this, eventHandlers.thumbDrag);
        $(document).on('mouseup', this, eventHandlers.thumbDragEnd);
    }

    // Create an element and merge attribute objects to attributes
    Rangeslider.prototype.createElement = function (tagName, className) {
        let attributes = $.extend.apply({}, Array.prototype.slice.call(arguments, 2));
        let element = $(tagName, attributes).addClass(className);

        return element;
    }

    // Remove the entire slider; resets the base element to its former state
    Rangeslider.prototype.remove = function () {
        this.element.removeClass('rangeslider');
        this.thumb.remove();
        this.track.remove();
        this.input.remove();
        this.element.removeData('rangeslider');
        this.element.off('click', eventHandlers.click);
        this.element.children().show();
    }

    // Get step width in pixels based on current page layout
    Rangeslider.prototype.getStepWidth = function () {
        return this.element.width() / this.stepCount;
    }

    // Set current step and value
    Rangeslider.prototype.setStep = function (step) {
        if (this.step !== step) {
            this.step = step;
            this.thumb.css('left', 'calc(' + 100 / this.stepCount * step + '% - ' + this.thumb.outerWidth(true) / 2 + 'px)');
            this.input.val(this.rangeStart + this.step * this.stepSize);

            this.element.trigger('rangeslider.valueChanged', this.getValue());
        }
    }

    // Get current value
    Rangeslider.prototype.getValue = function () {
        return this.rangeStart + this.stepSize * this.step;
    }

    // Set current value
    Rangeslider.prototype.setValue = function (value) {
        if (value < this.rangeStart) {
            this.setStep(0);
        }
        else if (value > this.rangeStart + this.stepCount * this.stepSize) {
            this.setStep(this.stepCount);
        }
        else {
            this.setStep(Math.round((value - this.rangeStart) / this.stepSize));
        }
    }

    // Event handlers should not be accessible from the object itself
    let eventHandlers = {
        thumbDragStart: function (e) {
            if (e.which !== 1) {
                return;
            }

            e.data.dragStatus.dragging = true;
            e.data.dragStatus.startPosition = e.pageX;
            e.data.dragStatus.startStep = e.data.step;
        },
        thumbDrag: function (e) {
            if (!e.data.dragStatus.dragging) {
                return;
            }

            let delta = e.data.dragStatus.startPosition - e.pageX;
            let step = e.data.dragStatus.startStep - Math.round(delta / e.data.getStepWidth());

            if (step < 0) {
                step = 0;
            }
            else if (step > e.data.stepCount) {
                step = e.data.stepCount;
            }

            e.data.setStep(step);
        },
        thumbDragEnd: function (e) {
            e.data.dragStatus.dragging = false;
        },
        click: function (e) {
            // If we're clicking the thumb, ignore the click here
            if (e.data.thumb.filter(e.target).length === 1) {
                return;
            }

            let step = Math.round(e.offsetX / e.data.getStepWidth());

            e.data.setStep(step);
        }
    };

}(jQuery));